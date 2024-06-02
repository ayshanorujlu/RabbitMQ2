"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

connection.start().then(function () {
    console.log("Started first")
}).catch(function (err) {
    return console.error(err.toString());
})


connection.on("Connect", function (info) {
    console.log("Connect Work first");
})
connection.on("Disconnect", function (info) {
    console.log("DisConnect Work first");
})


connection.on("newmessage", function (obj) {
    console.log("newmessaggee ",obj)
    let routingKey = obj.routingKey
    let message = obj.message

    $('#messages').append(`<h4>${routingKey} - ${message}</h4>`)

})



var checkboxes = document.querySelectorAll('input[type="checkbox"][name="color"]');
checkboxes.forEach((checkbox) => {
    checkbox.addEventListener("change", function () {
        if (checkbox.checked) {
            checkbox.parentElement.style.backgroundColor = "orange";
        } else {
            checkbox.parentElement.style.backgroundColor = "";
        }
    });
});

document.getElementById("selectForm2").addEventListener("submit", function (event) {
    event.preventDefault();
    var selecteds = [];

    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            checkbox.parentElement.style.backgroundColor = "lightgreen";
            selecteds.push(checkbox.value)
        } else {
            checkbox.parentElement.style.backgroundColor = "";
            selecteds = selecteds.filter(a => a != checkbox.value)
        }
    });


    if (selecteds.length == 0) {
        alert("No Selected")
        return;
    }

    $.ajax({
        url: '/Home/SaveSelects',
        method: 'POST',
        data: { selects: selecteds },
        success: function (response) {
            console.log('Colors saved successfully');
        },
        error: function (err) {
            console.error('Error saving colors', err);
        }
    });
});





