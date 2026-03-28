const apiRoot = "socket";
/**
 * 创建socket
 * @param {string} name 
 * @param {string} uri 
 * @param {string} password
 * @returns { Response } 
 */
async function create(name, uri, password = ""){
    return await fetchPost(`${apiRoot}/create`, JSON.stringify({ name: name, uri: uri, password: password }));
}

/**
 * 获取当前的全部已启用socket
 * @returns {   { name: string, uri: string, password: string, isEnable: boolean }[] | undefined }
 */
async function socketList(){
    /**
     * @type { Response }
     */
    let response = await fetchPost(`${apiRoot}/socketList`);
    if(typeof response == typeof 1) {
        return undefined
    }
    return await response.json();
}