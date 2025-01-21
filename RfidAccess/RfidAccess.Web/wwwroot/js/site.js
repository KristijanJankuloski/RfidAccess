const connection = new signalR.HubConnectionBuilder().withUrl("/notifications").build();

let notifications = [];
function addNotification(message) {
    const wrapper = document.getElementById("notification-card-wrapper");
    if (wrapper)
        wrapper.innerHTML = "";

    const notification = JSON.parse(message);

    if (notifications.length > 5) {
        notifications.pop();
    }

    notifications.unshift(notification);
    let body = `
        <div class="card">
        <div class="card-header">
            <h5>Недозволен влез</h5>
        </div>
        <div class="card-body">
            <table class="table">
                <thead>
                    <tr>
                        <th>Име</th>
                        <th>Број на картичка</th>
                        <th style="width: 15rem;">Време</th>
                    </tr>
                </thead>
                <tbody>         
    `
    notifications.forEach(n => {
        body += `
        <tr class="table-danger">
            <td>${n.Message}</td>
            <td>${n.Code}</td>
            <td>${n.Date}</td>
         </tr>`;
    });
    body += `</tbody>
            </table>
        </div>
    </div>`;

    if (wrapper) {
        wrapper.innerHTML = body;
    }
}
connection.on("Notification", (message) => addNotification(message));

connection.on("Confirmation", (message) => {
    const notification = JSON.parse(message);
    window.location.reload();
});

connection.on("Error", (message) => {
    const error = JSON.parse(message);
    toastr.error(error.Message, '', { "positionClass": "toast-bottom-right" });
});

connection.on("Warning", (msg) => {
    const warning = JSON.parse(msg);
    toastr.warning(warning.Message, '', { "positionClass": "toast-bottom-right" })
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});