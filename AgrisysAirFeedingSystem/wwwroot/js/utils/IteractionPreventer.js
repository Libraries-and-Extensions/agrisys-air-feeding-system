document.querySelectorAll("[data-long-press]").forEach(function (element) {    
    let timeoutID;
    
    let actionStr = element.dataset.longPress;
    let action = actionStr !== "" ? actionStr : 1000;
    
    let timeout = parseInt(element.dataset.longPressTimeout);
    timeout = !isNaN(timeout) ? timeout : 1000;
    
    if (element.dataset.longPressPreventClick.toLowerCase() === "true") {
        element.addEventListener("click", function (e) {
            e.preventDefault();
        })
    }
    

    element.addEventListener("mousedown", function (e) {
        if (e.isComposing || e.keyCode === 229) {
            return;
        }
        
        timeoutID = setTimeout(function () {
            location.assign(action);
            
        }, timeout);
    })

    element.addEventListener("mouseup", function (e) {
        if (e.isComposing || e.keyCode === 229) {
            return;
        }      

        clearTimeout(timeoutID);
    })

    element.addEventListener("mouseleave", (e) => {
        if (e.isComposing || e.keyCode === 229) {
            return;
        }

        clearTimeout(timeoutID);
    });
});