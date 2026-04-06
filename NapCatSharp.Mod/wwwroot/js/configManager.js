const apiroot = 'modconfig';
/**
 * 
 * @returns { response }
 */
function getConfigs() {
    return fetchPost(`${apiroot}/getconfigs`)
}

/**
 * 
 * @param {string} modName 模组名称 
 * @param {Object} config 配置值
 * @param {string} configName 配置名称
 */
function updateConfig(modName, config, configName){
    return fetchPost(`${apiroot}/updateconfig`, JSON.stringify({ modName: modName, config: config, configName: configName }));
}