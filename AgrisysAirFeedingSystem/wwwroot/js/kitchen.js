$(function() {
    $("#dosertestbtn").click(function () {
       $.get("/Sensor/Collect?key=C&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=C&value=0"); 
       }, 5000);
    });
    $("#mixerstirrertestbtn").click(function () {
        $.get("/Sensor/Collect?key=7&value=1");
        setTimeout(function () {
            $.get("/Sensor/Collect?key=7&value=0");
        }, 5000);
    });
    $("#mixerweighttestbtn").click(function () {
       $.get("/Sensor/Collect?key=8&value=5000"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=8&value=0"); 
       }, 5000);
    });
    $("#hatchtestbtn").click(function () {
       $.get("/Sensor/Collect?key=9&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=9&value=0"); 
       }, 5000);
    });
    $("#distributorweighttestbtn").click(function () {
        $.get("/Sensor/Collect?key=6&value=5000");
        setTimeout(function () {
            $.get("/Sensor/Collect?key=6&value=0");
        }, 5000);
    });
    $("#distributorstirrertestbtn").click(function () {
        $.get("/Sensor/Collect?key=5&value=1");
        setTimeout(function () {
            $.get("/Sensor/Collect?key=5&value=0");
        }, 5000);
    });
    $("#cellsluicetestbtn").click(function () {
       $.get("/Sensor/Collect?key=A&value=1"); 
       setTimeout(function () {
           $.get("/Sensor/Collect?key=A&value=0"); 
       }, 5000);
    });
    $("#blowertestbtn").click(function () {
        $.get("/Sensor/Collect?key=1&value=1");
        $.get("/Sensor/Collect?key=2&value=150");
        $.get("/Sensor/Collect?key=3&value=50");
        $.get("/Sensor/Collect?key=4&value=100");
        setTimeout(function () {
            $.get("/Sensor/Collect?key=1&value=0");
            $.get("/Sensor/Collect?key=2&value=0");
            $.get("/Sensor/Collect?key=3&value=0");
            $.get("/Sensor/Collect?key=4&value=0");
        }, 5000);
    });
});
