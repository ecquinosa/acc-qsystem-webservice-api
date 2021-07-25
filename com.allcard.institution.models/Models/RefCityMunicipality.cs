using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.models
{
    public class RefCityMunicipality : BaseEntity
    {
        public string ProvinceCode { get; set; }
        public string CityMunicipalityCode { get; set; }
        public string Description { get; set; }
    }
}
