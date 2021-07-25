package com.allcard.kyc.models;

import java.sql.Timestamp;
import java.util.Date;
import java.util.UUID;

import org.springframework.data.annotation.Id;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Getter @Setter 
public class MemberCategory  {
	@Id
	public Integer ID;
	public UUID GUID;
	public String EmployeeID;
	public String EmployerName;
	public String EmployerHBUR;
	public String EmployerBuilding;
	public String EmployerLogNo;
	public String EmployerBlockNo;
	public String EmployerPhaseNo;
	public String EmployerHouseNo;
	public String EmployerStreetName;
	public String EmployerSubdivision;
	public String EmployerBarangay;
	public String EmployerPSGCBarangayCode;
	public String EmployerCityMunicipality;
	public String EmployerPSGCCityMunicipalityCode;
	public String EmployerProvince;
	public String EmployerPSGCProvinceCode;
	public String EmployerRegion;
	public String EmployerPSGCRegionCode;
	public Date DateEmployed;
	public String Occupation;
	public String OccupationCode;
	public String AFPSerialBadgeNo;
	public String DepEdDivCodeStncode;
	public String TypeOfWork;
	public String IncomeCode;
	public String CountryAssignment;
	public String PSGCCountryAssignmentCode;
	public String NatureOfWork;
	public String OFWCountryCode;
	public String EmpStatusCode;
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
