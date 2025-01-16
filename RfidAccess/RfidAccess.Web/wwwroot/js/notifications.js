const connection = new signalR.HubConnectionBuilder().withUrl("/notifications").build();
const wrapper = document.getElementById("notification-card-wrapper");
connection.on("Notification", (message) => {
    const notification = JSON.parse(message);
    const body = `
        <div class="card">
        <div class="card-header">
            <h5>Недозволен влез</h5>
        </div>
        <div class="card-body">
            <table class="table">
                <thead>
                    <tr>
                        <th>Име</th>
                        <th>Шифра</th>
                        <th style="width: 15rem;">Време</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="table-danger">
                        <td>${notification.Message}</td>
                        <td>${notification.Code}</td>
                        <td>${notification.Date}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    `
    if (wrapper) {
        wrapper.innerHTML = body;
    }
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});