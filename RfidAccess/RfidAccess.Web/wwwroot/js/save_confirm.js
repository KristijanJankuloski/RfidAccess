const connection = new signalR.HubConnectionBuilder().withUrl("/notifications").build();

connection.on("Confirmation", (message) => {
    const notification = JSON.parse(message);
    window.location.reload();
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});