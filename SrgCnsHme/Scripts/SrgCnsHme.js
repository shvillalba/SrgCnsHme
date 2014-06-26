/// <reference path="jquery-1.9.1.intellisense.js" />
/// <reference path="bootstrap.js" />
/// <reference path="knockout-2.2.1.debug.js" />

function showRes(res) {
  alert(JSON.stringify(res));
}

//Commented out since we're not using jQueryUi
//$(function () {
//  $("[data-draggable]").draggable();
//});

function resObj(res) {
  // var html = $("<div></div>").append(res.responseText);  //LEARN: trick to be able to see (inserted in a div) html markup. Works, even for troublesome html, like this one, with <html> and <body> tags.
  var html = res.responseText;
  //$(html).each(function (idx, elt) { var tsttxt = $("#tsttxt").val(); $("#tsttxt").val(tsttxt + idx + ": " + this.outerHTML + "\n"); });   //LEARN: $(html) returns the elements INSIDE the <head> and inside the <body> tags: 1) <meta ..> 2) <title ..> 3) <p> .. </p>
  //var info = $(html).find("p").text(); //LEARN: does NOT work! (because '.find' searches INSIDE each of the elements returned by $(html): 1) INSIDE "<meta ..", 2) INSIDE "<title ..", 3) INSIDE <p> [and of course: there is no p within the single <p> here]
  var info = $(html).filter("p").text();
  //var info = $("<div></div>").append(html).find("p").text(); //LEARN: another option.
  return JSON.parse(info);
}

function sameUtcDates(dt1, dt2) {
  var d1 = dt1.getUTCDate();
  var d2 = dt2.getUTCDate();
  var m1 = dt1.getUTCMonth();
  var m2 = dt2.getUTCMonth();
  var y1 = dt1.getUTCFullYear();
  var y2 = dt2.getUTCFullYear();
  return (d1 == d2 && m1 == m2 && y1 == y2);
}