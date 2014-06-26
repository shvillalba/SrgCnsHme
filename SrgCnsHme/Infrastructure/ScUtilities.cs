using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using SrgCnsHme.Models;

namespace SrgCnsHme.Infrastructure
{
  public class ScUtilities
  {
    public class CString
    {
      public string name { get; set; }
      public string connString { get; set; }
      public string provider { get; set; }
    }

    public enum OpStatus
    {
      Success, NoAction, Error
    }

    static public CustomErrorsMode GetCustomErrorsMode()
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var section = (CustomErrorsSection)conf.GetSection("system.web/customErrors");
      return section != null ? section.Mode : CustomErrorsMode.RemoteOnly;
    }

    static public CustomErrorsMode CurrentCustomErrorsMode()
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var section = (CustomErrorsSection)conf.GetSection("system.web/customErrors");
      return section.Mode;
    }

    static public OpStatus ChangeCustomErrorsMode(CustomErrorsMode newMode)
    {
      try
      {
        var conf = WebConfigurationManager.OpenWebConfiguration("~");
        var section = (CustomErrorsSection)conf.GetSection("system.web/customErrors");
        if (section != null)
        {
          section.Mode = newMode;
          conf.Save();
        }
        return OpStatus.Success;
      }
      catch
      {
        return OpStatus.Error;
      }
    }

    static public OpStatus AddConnectionString(string stringName, string connectionString, string providerName, bool updateInitTbl = false)
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var section = (ConnectionStringsSection)conf.GetSection("connectionStrings"); //LEARN: you need the connection strings as a "ConfigurationSection", so you can write (see below)
      var result = OpStatus.Success;

      try
      {
        var str = new ConnectionStringSettings(stringName, connectionString, providerName);
        section.ConnectionStrings.Remove(str);
        section.ConnectionStrings.Add(str);
        conf.Save();

        if (updateInitTbl)
        {
          var db = new DefaultEfContext();
          var cst = new Models.SrgCnsHme();
          cst.StringName = stringName;
          cst.ConnectionString = connectionString;
          cst.ProviderName = providerName;
          db.SrgCnsHmes.Add(cst);
          db.SaveChanges();
        }
      }
      catch
      {
        result = OpStatus.Error;
      }
      return result;
    }

    static public OpStatus AddOrUpdateConnectionString(string stringName, string connectionString, string providerName, bool updateInitTbl = false)
    {
      var result = OpStatus.NoAction;
      var cs = WebConfigurationManager.ConnectionStrings[stringName];
      var difVal = !cs.ToString().Equals(connectionString);
      var difPrv = !cs.ProviderName.Equals(providerName);
      if (cs == null || difVal || difPrv)
      {
        result = AddConnectionString(stringName, connectionString, providerName);
        if (updateInitTbl)
        {
          var db = new DefaultEfContext();
          var cst = db.SrgCnsHmes.Single((c => c.StringName == stringName));
          if (cst != null)
          {
            cst.StringName = stringName;
            cst.ConnectionString = connectionString;
            cst.ProviderName = providerName;
            db.Entry(cst).State = EntityState.Modified;
            db.SaveChanges();
          }
        }
      }
      return result;
    }

    static public OpStatus DeleteConnectionString(string stringName, bool updateInitTbl = false)
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var section = (ConnectionStringsSection)conf.GetSection("connectionStrings");
      var result = OpStatus.Success;

      try
      {
        section.ConnectionStrings.Remove(stringName);
        conf.Save();

        if (updateInitTbl)
        {
          var db = new DefaultEfContext();
          var cst = db.SrgCnsHmes.Single(c => c.StringName == stringName);
          if (cst != null)
          {
            db.SrgCnsHmes.Remove(cst);
            db.SaveChanges();
          }
        }
      }
      catch (Exception)
      {
        result = OpStatus.Error;
      }
      return result;
    }

    static public void InitializeDbConnectionStrings(HttpServerUtility server)
    {
      var connStrings = WebConfigurationManager.ConnectionStrings;  //LEARN: remember the "ConnectionStrings" collection is read-only!!
      if (connStrings.Count > 2 && connStrings["DefaultConnection"].ToString() != "")
        return;

      string defaultConnection;
      try
      {
        defaultConnection = File.ReadAllText(server.MapPath("ah.shv"));
        AddConnectionString("DefaultConnection", defaultConnection, "System.Data.SqlClient");
      }
      catch
      {
        // We get here when we're running in AppHarbor and the 'ah.shv' file is not found, since this file is not deployed and used only locally.
        defaultConnection = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
      }
      var conn = new SqlConnection(defaultConnection);
      var cmd = new SqlCommand("select * from dbo.SrgCnsHme;", conn);
      try
      {
        conn.Open();

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          var stringName = reader["StringName"].ToString();
          var connectionString = reader["ConnectionString"].ToString();
          var providerName = reader["ProviderName"].ToString();

          AddConnectionString(stringName, connectionString, providerName);
        }
        reader.Close();
      }
      catch
      {
        AddConnectionString("DefaultConnection", "", "System.Data.SqlClient"); //Try to return the ConnectionStrings in its start-up state.
      }
      finally
      {
        EncryptDecryptConnectionStrings(true);
        cmd.Dispose();
        conn.Close();
      }
    }

    static public CString[] ReadConnStringsFromWebConfig()
    {
      var connectionStrings = WebConfigurationManager.ConnectionStrings;
      var connectionStringsEnum = connectionStrings.GetEnumerator();

      var connStrings = new List<CString>();

      var i = 0;
      while (connectionStringsEnum.MoveNext())
      {
        var name = connectionStrings[i].Name;
        var connString = connectionStrings[name].ToString();
        var provider = connectionStrings[i].ProviderName;
        connStrings.Add(new CString { name = name, connString = connString, provider = provider });
        i++;
      }
      return connStrings.ToArray();
    }

    static public bool ConnStringsAreEncrypted()
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var csSection = conf.GetSection("connectionStrings");
      return csSection.SectionInformation.IsProtected;
    }

    static public OpStatus EncryptDecryptConnectionStrings(bool encrypt)
    {
      var conf = WebConfigurationManager.OpenWebConfiguration("~");
      var section = conf.GetSection("connectionStrings");
      OpStatus result;

      try
      {
        if (encrypt)
        {
          if (!section.SectionInformation.IsProtected)
          {
            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            result = OpStatus.Success;
          }
          else
            result = OpStatus.NoAction;
        }
        else
        {
          if (section.SectionInformation.IsProtected)
          {
            section.SectionInformation.UnprotectSection();
            result = OpStatus.Success;
          }
          else
            result = OpStatus.NoAction;
        }
        conf.Save();
      }
      catch (Exception ex)
      {
        result = OpStatus.Error;
      }
      return result;
    }
  }
}