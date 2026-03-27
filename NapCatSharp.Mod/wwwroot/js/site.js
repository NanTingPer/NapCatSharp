// jsencrypt@3.5.4

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
import { JSEncrypt } from "../lib/jsencrypt/jsencrypt";
function encryptedPassword(password, publicKey)
{
    // 获取公钥
    const pubKey = await fetch('/login/publicKey').then(res => res.json());
    const encryptor = new JSEncrypt();
    encryptor.setPublicKey(pubKey.publicKey);

    const password = password;
    const encrypted = encryptor.encrypted(password);

    fetch('/api/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ requestId: pubKey.requestId, password: password })
    });
}