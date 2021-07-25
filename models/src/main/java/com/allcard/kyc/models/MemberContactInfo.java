package com.allcard.kyc.models;

import java.sql.Timestamp;
import java.util.Date;
import java.util.UUID;

import org.springframework.data.annotation.Id;

public class MemberContactInfo {
	@Id
	public Integer ID;
	public UUID GUID;
	public String PermanentName;
	public String PermanentHBUR;
	public String PermanentBuilding;
	public String PermanentLogNo;
	public String PermanentBlockNo;
	public String PermanentPhaseNo;
	public String PermanentHouseNo;
	public String PermanentStreetName;
	public String PermanentSubdivision;
	public String PermanentBarangay;
	public String PermanentPSGCBarangayCode;
	public String PermanentCityMunicipality;
	public String PermanentPSGCCityMunicipalityCode;
	public String PermanentProvince;
	public String PermanentPSGCProvinceCode;
	public String PermanentRegion;
	public String PermanentPSGCRegionCode;
	public String PresentHBUR;
	public String PresentBuilding;
	public String PresentLogNo;
	public String PresentBlockNo;
	public String PresentPhaseNo;
	public String PresentHouseNo;
	public String PresentStreetName;
	public String PresentSubdivision;
	public String PresentBarangay;
	public String PresentPSGCBarangayCode;
	public String PresentCityMunicipality;
	public String PresentPSGCCityMunicipalityCode;
	public String PresentProvince;
	public String PresentPSGCProvinceCode;
	public String PresentRegion;
	public String PresentPSGCRegionCode;
	
	public String HomeCountryCode;
	public String HomeAreaCode;
	public String HomeTelNo;
	public String MobileCountryCode;
	public String MobileAreaCode;
	public String MobileCellNo;
	public String BusinessDirectCountryCode;
	public String BusinessDirectAreaCode;
	public String BusinessDirectCellNo;
	public String BusinessTrunkCountryCode;
	public String BusinessTrunkAreaCode;
	public String BusinessTrunkCellNo;
	public String Email;
	public Integer CreatedBy;
	public Date CreatedDate;
	public Integer UpdatedBy;
	public Date UpdatedDate;
	public Timestamp Version;
	
	// Relationship
	public Integer MemberID;
	public Member Member;
	// End Relationship.
}
