document.querySelectorAll("[data-postback]").forEach(element => {
    let url = element.dataset.postback;
    let parameter = element.dataset.postbackParameter;
    let fetchMethod = element.dataset.postbackMethod ?? "POST";

    if ((url ?? parameter) == null) {
        return;
    }

    if (url != null && url.indexOf("&") > 0)
        url += "&";
    else
        url += "?";

    let dataStr = element.dataset.postbackRequestData;
    let data = dataStr === undefined ? {} : dataStr.split(";").reduce(
        (obj, item) => {
            let parts = item.split(":");
            obj[parts[0]] = parts[1];
            return obj;
        }
        ,{})

    let oldValue = element.value;

    element.addEventListener("change", function () {
        console.log(element.value);

        data["old"+parameter] = oldValue;
        data[parameter] = element.value;
        oldValue = element.value;
        
        if (fetchMethod === "GET") {
            url += "?" + parameter + "=" + element.value;
        }

        fetch(url, {
            method: fetchMethod,
            body: JSON.stringify(data),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then( async response => {
            console.log(response);
            
            if (response.status === 204) {
                showToast("Success", "Role changed to " + element.value, "success")
            }else if (response.status === 400) {
                let error = await response.json();
                showToast("Error", error.text, "danger")
            }
        }).catch(async error => {
            console.log(error);
            console.log(error.text);
            showToast("Error", error.text, "danger")
        });
    });
});