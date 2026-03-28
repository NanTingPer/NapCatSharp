// jsencrypt@3.5.4

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
async function encryptedPassword(password, publicKey)
{
    const encryptor = new JSEncrypt();
    encryptor.setPublicKey(publicKey);
    const evalue = encryptor.encrypt(password);
    return evalue;
}

/** 
 * 刷新JWT并缓存到 localStorage['jwt'] 
 * 
 * return bool 如果true则成功，否则失败
*/
async function flushToken(password = "") {
    /**
     *  按照流程
     *  - 先获取公钥 -> 会返回requestid 登录时使用
     *  - 使用公钥加密密钥
     *  - 使用加密后的密码登录，需要传入 requestid
     *  - 登录后返回jwt
     */
    let usePassword = "";
    if (password == "") {
        usePassword = sessionStorage['password'];
    } else {
        usePassword = password;
    }
    let pkresponse = await fetch(`${window.apiUrl}login/publicKey`,{
        method: "POST"
    });
    if(!(pkresponse.status == 200)){
        window.location.href = `${window.apiUrl}`;
        return false;
    }
    let obj = await pkresponse.json();
    let ps = await encryptedPassword(usePassword, obj.publicKey);
    let jwtResponse = await fetch(`${window.apiUrl}login/login`, {
        method: "POST",
        body: JSON.stringify({password: ps, requestId: obj.requestId}),
        headers: {
            "Content-Type" : "application/json"
        }
    });
    if (!(jwtResponse.status == 200)){
        window.location.href = `${window.apiUrl}`;
        return false;
    }
    let jwt = (await jwtResponse.json()).token;
    localStorage['jwt'] = jwt;
    return true;
}

/**
 * 发送api请求，请求url
 * @param { string } url 请求相对路径 不要带起始'/'
 * @param { number } recount 重试次数 
 * @param { BodyInit | null } body 请求体
 * @returns 成功则返回 response, 失败返回 { status: 404 }
 */
async function fetchPost(url, body, recount = 1, ) {
    let count = 0;
    console.log(body)
    do {
        let response = await fetch(`${window.apiUrl}${url}`, 
        { 
            method: "POST",
            body: body == null || body == undefined ? null : body,
            headers: {
                "Content-Type" : "application/json",
                "Authorization" : ` ${localStorage['jwt']}`
            }
        });
        if(response.status == 200){
            return response;
        }
        await flushToken();
        console.log("刷新Token");
        count++;
    } while (count < recount + 1); // 最少也要一次 无感刷新token
    return { status: 404 };
}