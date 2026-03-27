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