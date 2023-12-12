$(function() {
    $("#dosertestbtn").click(function () {
       $.get("/Sensor/Collect?key=2&value=100"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=2&value=0"); 
       }, 5000);
    });
    $("#mixerstirrertestbtn").click(function () {
       $.get("/Sensor/Collect?key=6&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=6&value=0"); 
       }, 5000);
    });
    $("#hatchtestbtn").click(function () {
       $.get("/Sensor/Collect?key=8&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=8&value=0"); 
       }, 5000);
    });
    $("#distributorstirrertestbtn").click(function () {
       $.get("/Sensor/Collect?key=4&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=4&value=0"); 
       }, 5000);
    });
    $("#cellsluicetestbtn").click(function () {
       $.get("/Sensor/Collect?key=9&value=100"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=9&value=0"); 
       }, 5000);
    });
    $("#blowertestbtn").click(function () {
       $.get("/Sensor/Collect?key=2&value=100"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=2&value=0"); 
       }, 5000);
    });
});
