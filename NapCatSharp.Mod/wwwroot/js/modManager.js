/**  */
class AppendModFileInput {
    /** 模组名称 */
    modName;
    /** 文件大小 */
    fileSize;
    /** 文件名称 */
    fileName;
    /** 资源内容 */
    base64;
    /** 是否结束 */
    isEnd;
    /**
     * 使用Json.stringify
     * @returns { { modName: string, fileSize: number, fileName: string, base64: string, isEnd: boolean } }
     */
    toJson() {
        return JSON.stringify({
            modName: this.modName,
            fileSize: this.fileSize,
            fileName: this.fileName,
            base64: this.base64,
            isEnd: this.isEnd
        });
    }
}
const apiroot = 'modmanager';

/**
 * 删除给定模组文件
 * @param { string } modName 
 */
async function deletemodfiles(modName) {
    return await fetchPost(`${apiroot}/deletemodfiles`, JSON.stringify({ modname: modName }));
}

/**
 * @param { string } modName
 * @param { File[] | FileList } files
 * @returns { string } 
 */
async function appendModFile(modName, files) {
    let modRootFile;
    /**
     * @@type { File[] }
     */
    let fileArray = [];
    for(let i = 0; i < files.length; i++){
        let file = files[i];
        if(file.name.split('.dll')[0] == modName){
            modRootFile = file;
        }
        fileArray.push(file);
    }
    if (modRootFile == undefined) {
        return "未找到模组主文件，即: 与模组名称一致的dll文件";
    }

    /**
     * @type { Promise<Response> }
     */
    let response = fileArray.map(async f => {
        let fileBase64 = (await f.bytes()).toBase64(); //https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array/toBase64
        // let fileBase64 = btoa(new TextDecoder().decode(await f.bytes()))
        var input = new AppendModFileInput();
        input.fileName = f.name;
        input.fileSize = f.size;
        input.isEnd = true;
        input.modName = modName;
        input.base64 = fileBase64;
        return await fetchPost(`${apiroot}/appendmodFile`, input.toJson());
    });

    /**
     * @type { Response[] }
     */
    let responses = [];
    for(let i = 0; i < response.length; i++){
        responses.push(await response[i]);
    }

    let error = responses.find(f => f.status != 200);
    if(error != undefined) {
        await deletemodfiles(modName);
        return `上传失败${(await error.json()).errorMsg ?? "无法读取错误信息"}`;
    }
    return "上传成功";
}

/**
 * @returns { Response } 获取已启用的模组列表
 */
async function modList() {
    return await fetchPost(`${apiroot}/modlist`);
}

/**
 * @param { string } modName 
 * @returns { Response } 
 */
async function disablemod(modName) {
    return await fetchPost(`${apiroot}/disablemod`, JSON.stringify({ modname: modName }));
}

/**
 * @returns { Response } 获取本地模组列表
 */
async function localMods() {
    return await fetchPost(`${apiroot}/localMods`);
}

/**
 * 重新加载给定模组
 * @param { string } modName 
 * @returns { Response }
 */
async function reloadmod(modName) {
    return await fetchPost(`${apiroot}/reloadmod`, JSON.stringify({ modname: modName }));
}

/**
 * 启用给定模组
 * @param { string } modName 
 * @returns { Response }
 */
async function enablemod(modName) {
    return await fetchPost(`${apiroot}/enablemod`, JSON.stringify({ modname: modName }));
}