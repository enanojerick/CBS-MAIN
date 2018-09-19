using System;
using System.ComponentModel.DataAnnotations;


namespace CBS.Data.Entities
{
    public class CBS_Application : BaseEntity
    {
        [Key]
        public int? AppID { get; set; }

        public string URL { get; set; }

        public string ClientID { get; set; }

        public string ClientName { get; set; }

        public string Sha256 { get; set; }

        public string RedirectURIs { get; set; }

        public string PostLogoutRedirectUris { get; set; }

        public string CorsOrigins { get; set; }

        public string ReturnURL { get; set; }
    }
}
