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

/**
 * 获取已禁用的socket
 * @returns {   { name: string, uri: string, password: string, isEnable: boolean }[] | undefined }
 */
async function disableList(){
    /**
     * @type { Response }
     */
    let response = await fetchPost(`${apiRoot}/disableList`);
    if(typeof response == typeof 1) {
        return undefined
    }
    return await response.json();
}

/**
 * 启用给定socket
 * @param { string } name 
 */
async function enable(name){
        /**
     * @type { Response }
     */
    let response = await fetchPost(`${apiRoot}/enable`, JSON.stringify( { name: name }));
}

/**
 * 删除给定socket
 * @param {string} name
 */
async function deleted(name){
    let response = await fetchPost(`${apiRoot}/delete`, JSON.stringify( { name: name }));
}

/**
 * 禁用给定socket
 * @param {string} name
 */
async function disable(name){
    let response = await fetchPost(`${apiRoot}/disable`, JSON.stringify( { name: name }));
}