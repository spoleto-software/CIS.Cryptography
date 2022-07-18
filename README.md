# CIS.Cryptography

CIS project for Windows cryptography API via Interop.

## Quick start

In order to sign a data as Base64 format you need to pass a certificate thumbprint.

```
var base64stringToSign = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToConvert));
var signedData = CIS.Cryptography.CryptographyHelper.SignBase64Data(data, thumbprint: settings.CertificateThumbprint);
```
