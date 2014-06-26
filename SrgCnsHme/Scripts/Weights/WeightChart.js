/// <reference path="../moment.js" />
/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.js" />
/// <reference path="../../Scripts/linq-vsdoc.js" />
/// <reference path="../../BootstrapSupport/js/bootstrap.js" />
/// <reference path="../../BootstrapSupport/js/bootstrap-datepicker.js" />
/// <reference path="../date.format.js" />
/// <reference path="../SrgCnsHme.js" />

var today = new Date();
var weekAgo = new Date().setDate(today.getDate() - 7);
var monthAgo = new Date().setDate(today.getDate() - 30);
var strToday = today.format('d-mmm-yyyy');
var oldestDate, startTableDate;

function dateOutOfWtRange(d) {
  return d < oldestDate || d > today;
}

function dateHasWt(d, wts) {
  var wtIn = false;
  $.each(wts, function (idx, wt) {
    var df = d.format('d-mmm-yyyy');
    if (df == wt.date()) {
      wtIn = true;
      return false;
    }
    return true;
  });
  return wtIn;
}

function dateIn(utcDt) {
  var dbind = this.element.attr("data-bind");  //LEARN: "this" is the datepicker object, and its "element" attribute points to its jQuery wrapped dom object  (see line #32 of bootstrap-datepicker.js)
  if (dbind.indexOf("newSrgDate") >= 0)
    return dateHasWt(utcDt, wpm.srgWeights());
  if (dbind.indexOf("newCslDate") >= 0)
    return dateHasWt(utcDt, wpm.cslWeights());
  return dateOutOfWtRange(utcDt);
}

function getStartDate() {
  var dbind = $(this).attr("data-bind");  //LEARN: the dom object here needs $() wrapping before using jQuery with it.
  if (dbind.indexOf("newSrgDate") >= 0 || dbind.indexOf("newCslDate") >= 0)
    return startTableDate.format('d-mmm-yyyy');
  return oldestDate.format('d-mmm-yyyy');
}

function initDatepickers() {
  $(".datepicker").each(function () {
    var startDate = getStartDate.call(this); //LEARN: "this" (inside 'each') === dom object with .datepicker class
    $(this).datepicker({
      format: 'd-M-yyyy',
      trigger: 'byclick',
      startDate: startDate,
      endDate: strToday,
      autoclose: true,
      disableIfNoEnabledDates: true,
      dateIsDisabled: function (d) { //returns true/false if this date should be shown 'disabled'
        var dt = new Date();
        var localOffset = dt.getTimezoneOffset() * 60000; //hours of offset * 60k miliseconds/hour.
        var utcDt = new Date(d.getTime() + localOffset);
        return dateIn.call(this, utcDt); //LEARN: "this" (inside function invoked by datepicker) === the datepicker object *itself*
      }
    }); //.val("");
  });
}

function Weight(data) {
  var wght = data.weight.toFixed(1);
  this.isNew = ko.observable(data.isNew);
  this.name = ko.observable(data.name);
  this.date = ko.observable(data.date);
  this.weight = ko.observable(wght);
  this.markedForDeletion = ko.observable(false);

  // used during editing:
  this._date = ko.observable("");
  this._weight = ko.observable("");
  this.inEditMode = ko.observable(false);

  // original values (used to detect changes during edits, etc) They don't change, so they're not "observable".
  this._dateOr = data.date;
  this._weightOr = wght;
  this._id = data.id;

  this.amCurRow = ko.observable(false);
}

function WeightPageModel(srgWeights, cslWeights) {
  var me = this;
  var getWeightChartUrl, addNewWeightUrl, delWeightUrl, getWeightsUrl, saveWeightsUrl;

  function getPageInfo() {
    $.ajax("./Weights/PageInfo", {
      dataType: "json",
      type: "POST",
      success: function (res) {
        getWeightChartUrl = res.GetWeightChartUrl;
        addNewWeightUrl = res.AddNewWeightUrl;
        delWeightUrl = res.DelWeightUrl;
        getWeightsUrl = res.GetWeightsUrl;
        saveWeightsUrl = res.SaveWeightsUrl;
        me.fromDate(res.DefaultFromDate);
        me.toDate(res.DefaultToDate);
        var oDt = res.OldestWtDate.split("-");
        oldestDate = new Date(Date.parse(oDt[1] + " " + oDt[0] + " " + oDt[2]));
        var stDt = res.StartTableDate.split("-");
        startTableDate = new Date(Date.parse(stDt[1] + " " + stDt[0] + " " + stDt[2]));
        me.getWeightChart(getWeightChartUrl + "?person=both&fromDate=" + me.fromDate() + "&toDate=" + me.toDate() + "&time=" + new Date().getTime());
        me.srgWeights($.map(res.SrgWeights, function (wt) { return new Weight(wt); }));
        me.cslWeights($.map(res.CslWeights, function (wt) { return new Weight(wt); }));
        initDatepickers();
      }
    });
  };

  getPageInfo();

  me.refreshChart = function () {
    var radio = $("input:radio[name='personSelectionRadios']:checked");
    me.getWeightChart(getWeightChartUrl + "?person=" + radio.val() + "&fromDate=" + me.fromDate() + "&toDate=" + me.toDate() + "&time=" + new Date().getTime());

    // refresh weights in tables also
    $.post(getWeightsUrl, null, function (weights) {
      me.srgWeights([]);
      me.srgWeights($.map(weights.srgWeights, function (wt) { return new Weight(wt); }));
      me.cslWeights([]);
      me.cslWeights($.map(weights.cslWeights, function (wt) { return new Weight(wt); }));
      $("#newSrgDt,#newCslDt").datepicker("remove");
      initDatepickers();
    }, "json");

    //getPageInfo();
    return true;
  };

  me.currentRow = ko.observable();
  me.fromDate = ko.observable();
  me.toDate = ko.observable();
  me.getWeightChart = ko.observable();

  me.beginEdit = function () {
    var data = me.currentRow();
    data._date(data.date());
    data._weight(data.weight());
    data.inEditMode(true);
  };
  me.okEdit = function () {
    var data = me.currentRow();
    data._date("");
    data._weight("");
    data.inEditMode(false);
  };
  me.cancelEdit = function () {
    var data = me.currentRow();
    data.date(data._date()); //restore saved values, before this edit.
    data.weight(data._weight());
    me.okEdit();
  };
  me.restoreOriginal = function () {
    var data = me.currentRow();
    data.date(data._dateOr);
    data.weight(data._weightOr);
    me.okEdit();
  };
  me.curRowCanEditDel = ko.computed(function () {
    var curRow = me.currentRow();
    if (!curRow) return false;
    if (curRow.markedForDeletion()) return false;
    else return !curRow.inEditMode();
  });
  me.curRowInEditMode = ko.computed(function () {
    var curRow = me.currentRow();
    if (!curRow) return false;
    return curRow.inEditMode();
  });
  me.curRowMarkedForDeletion = ko.computed(function () {
    var curRow = me.currentRow();
    if (!curRow) return false;
    return curRow.markedForDeletion();
  });
  me.addNewWeight = function (name) {
    var date, weight;
    switch (name) {
      case "Sergio":
        date = me.newSrgDate();
        weight = me.acceptedNewSrgWeight();
        break;
      case "Consuelo":
        date = me.newCslDate();
        weight = me.acceptedNewCslWeight();
        break;
      default:
        alert("no such person");
        return;
    }
    $.ajax(addNewWeightUrl, {
      dataType: "json",
      data: { name: name, date: date, weight: weight },
      type: "POST",
      success: function (res) {
        //alert(res.status);
        $("#refreshChartBtn").click();
        me.newSrgWeight("");
        me.newCslWeight("");
      }
    });
  };
  me.delWeight = function () { //TODO: temporary: the delete logic should be like "maintenance" page's ('cross out' now ... del later)
    var data = me.currentRow();
    data.markedForDeletion(true);
    //$.ajax(delWeightUrl, {
    //  dataType: "json",
    //  data: { name: data.name(), date: data.date(), weight: data.weight() },
    //  type: "POST",
    //  success: function (res) {
    //    //alert(res.status);
    //    $("#refreshChartBtn").click();
    //  }
    //});
  };
  me.unDelWeight = function () {
    var data = me.currentRow();
    data.markedForDeletion(false);
  };
  me.selectRow = function (data) {
    if (me.currentRow())
      me.currentRow().amCurRow(false);
    data.amCurRow(true);
    me.currentRow(data);
    $("#opStatus").text("");
  };
  me.dblClick = function (data) {
    me.selectRow(data);
    me.beginEdit();
  };
  me.keyUp = function (data, event) {
    switch (event.keyCode) {
      case 13:
        me.okEdit();
        break;
      case 27:
        me.cancelEdit();
        break;
    }
  };

  function doToAll(f) { var allWeights = $.merge($.merge([], me.srgWeights()), me.cslWeights()); $.each(allWeights, f); }
  function nameToId(name) {
    switch (name) {
      case "Sergio":
        return 1;
      case "Consuelo":
        return 2;
      default:
        return -1;
    }
  }

  me.submitWeightChanges = function () {
    var weightsToDel = [];
    var weightsToEdit = [];
    doToAll(function (i, w) {
      if (w.markedForDeletion())
        weightsToDel.push({ WeightId: w._id, PersonId: nameToId(w.name()), Date: w.date(), WeightInLb: w.weight() });
    });
    doToAll(function (i, w) {
      if (w.weight() != w._weightOr)
        weightsToEdit.push({ WeightId: w._id, PersonId: nameToId(w.name()), Date: w.date(), WeightInLb: w.weight() });
    });
    $.post(saveWeightsUrl,
      { weightsToDel: ko.toJSON(weightsToDel), weightsToEdit: ko.toJSON(weightsToEdit) },
      function (res) {
        $("#opStatus").text(res.status);
      });
    me.clearWeightChanges();
    me.refreshChart();
  };
  me.clearWeightChanges = function () { doToAll(function (i, w) { w.markedForDeletion(false); w.inEditMode(false); }); };
  //------------------------- Sergio --------------------------------------------------------------
  me.newSrgDate = ko.observable(strToday);
  me.acceptedNewSrgWeight = ko.observable();
  me.newSrgWeightIsValid = ko.observable(false);

  me.newSrgWeight = ko.computed({
    read: me.acceptedNewSrgWeight,
    write: function (value) {
      if (value > 140 && value < 180) {
        me.newSrgWeightIsValid(true);
        me.acceptedNewSrgWeight(value);
      } else {
        me.newSrgWeightIsValid(false);
        if (value == "")
          me.acceptedNewSrgWeight(""); //auto-cleared after saving.
      }
    }
  });
  me.checkSrgWeightVal = function () {
    var sw = $("#newSrgWt").val();
    me.newSrgWeight(sw);
  };
  me.srgWeights = ko.observableArray();

  me.srgTodayWeightIsIn = ko.computed(function () {
    var isIn = false;  //true when today's weight "was already entered
    $.each(me.srgWeights(), function (i, w) {
      if (w.date() == strToday) {
        isIn = true;
        return false;
      }
      return true;
    });
    return isIn;
  });

  //----------------------------------- Consuelo --------------------------------------------------------------
  me.newCslDate = ko.observable(strToday);
  me.acceptedNewCslWeight = ko.observable();
  me.newCslWeightIsValid = ko.observable(false);

  me.newCslWeight = ko.computed({
    read: me.acceptedNewCslWeight,
    write: function (value) {
      if (value > 120 && value < 150) {
        me.newCslWeightIsValid(true);
        me.acceptedNewCslWeight(value);
      } else {
        me.newCslWeightIsValid(false);
        if (value == "")
          me.acceptedNewCslWeight("");
      }
    }
  });
  me.checkCslWeightVal = function () {
    var sw = $("#newCslWt").val();
    me.newCslWeight(sw);
  };
  me.cslWeights = ko.observableArray();

  me.cslTodayWeightIsIn = ko.computed(function () {
    var isIn = false;  //true when today's weight "was already entered
    $.each(me.cslWeights(), function (i, w) {
      if (w.date() == strToday) {
        isIn = true;
        return false;
      }
      return true;
    });
    return isIn;
  });

  //***********************************************************************************************************
  me.weightsChanged = ko.computed(function () {
    var hasChanges = false;
    doToAll(function (i, w) {
      if (w.markedForDeletion() || w.weight() != w._weightOr) {
        hasChanges = true;
        return false;
      }
      return true;
    });
    return hasChanges;
  });
}

var wpm;
wpm = new WeightPageModel();
ko.applyBindings(wpm);

