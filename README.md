# CIS.Cryptography

The project for Windows cryptography API via Interop.

## Quick start

In order to sign a data as Base64 format you need to pass a certificate thumbprint.

```
var dataToSign = Encoding.UTF8.GetBytes(stringToConvert);
var base64StringToSign = System.Convert.ToBase64String(dataToSign);
var signedData = CIS.Cryptography.CryptographyHelper.SignBase64Data(data, thumbprint: settings.CertificateThumbprint);
```
