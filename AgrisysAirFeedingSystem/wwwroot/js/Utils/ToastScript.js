const toastLiveExample = document.getElementById('liveToast')

const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)

let levels = {
    "success": "bg-success",
    "info": "bg-info",
    "warning": "bg-warning",
    "danger": "bg-danger"
}

function showToast(title, body,type) {
    toastLiveExample.classList.remove('hide')
    toastLiveExample.classList.remove('text-bg-primary')

    document.getElementById("toastBody").innerText = body
    document.getElementById("toastTitle").innerText = title

    toastBootstrap.show()
}