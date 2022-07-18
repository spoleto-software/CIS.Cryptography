using System.Text;
using CAPICOM;

namespace CIS.Cryptography
{
    /// <summary>
    /// Class to display Crypto certificates.
    /// Can be usefull in wpf custom combo box.
    /// </summary>
    public class CISCertificate
    {
        private ICertificate _certificate;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        public CISCertificate(ICertificate certificate)
        {
            _certificate = certificate;
        }

        /// <summary>
        /// Crypto COM certificate
        /// </summary>
        public ICertificate Certificate
        {
            get => _certificate;
            set => _certificate = value;
        }

        /// <summary>
        /// ToString. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ConvertToDisplay(_certificate.SubjectName)}от: {_certificate.ValidFromDate:d}, до: {_certificate.ValidToDate.Date:d}";
        }

        private string ConvertToDisplay(string str)
        {
            var categories = str.Split(',');
            string[] categoryParams = { "CN=", "SN=" };
            var res = new StringBuilder();
            foreach (var s in categories)
            {
                foreach (var cat in categoryParams)
                {
                    string name = s.Trim();
                    if (name.StartsWith(cat))
                    {
                        res.Append(name.Substring(cat.Length)).Append(", ");
                    }
                }
            }

            return res.ToString();
        }
    }
}
