
var checkboxes = document.querySelectorAll('input[type="checkbox"][name="color"]');


let list = [];




checkboxes.forEach((checkbox) => {
    checkbox.addEventListener("change", function () {
        if (checkbox.checked) {
            checkbox.parentElement.style.backgroundColor = "lightblue";
        } else {
            checkbox.parentElement.style.backgroundColor = "";
        }
    });
});

document.getElementById("selectForm1").addEventListener("submit", function (event) {
    event.preventDefault();

    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            checkbox.parentElement.style.backgroundColor = "lightgreen";
            let value = checkbox.value;
            if (list.indexOf(value) == -1) {
                list.push(checkbox.value)
            }
        } else {
            checkbox.parentElement.style.backgroundColor = "";
            list = list.filter(a => a != checkbox.value)
        }
    });


});



document.getElementById("messageForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let message = document.getElementById("messageInp").value;
    $.ajax({
        url: '/Home/SendMessage',
        method: 'POST',
        data: { selecteds: list, message: message },
        success: function (response) {
            console.log('Colors saved successfully');
        },
        error: function (err) {
            console.error('Error saving colors', err);
        }
    });
    this.reset()
});