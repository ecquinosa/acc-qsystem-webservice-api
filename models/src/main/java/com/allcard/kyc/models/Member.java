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
public class Member {
	@Id
	public Integer ID;
	public UUID GUID;
	public Integer InstitutionID;
	public String RefNumber;
	public String MemberLastmame;
	public String MemberFirstname;
	public String MemberMiddlename;
	public String MemberNoMiddlename;
	public String MemberExtension;
	public String BirthLastmame;
	public String BirthFirstname;
	public String BirthMiddlename;
	public String BirthNoMiddlename;
	public String BirthExtension;
	public String SpouseLastmame;
	public String SpouseFirstname;
	public String SpouseMiddlename;
	public String SpouseNoMiddlename;
	public String SpouseExtension;
	public String MotherLastmame;
	public String MotherFirstname;
	public String MotherMiddlename;
	public String MotherNoMiddlename;
	public String MotherExtension;
	public String FatherLastmame;
	public String FatherFirstname;
	public String FatherMiddlename;
	public String FatherNoMiddlename;
	public String FatherExtension;
	public Date Birthdate;
	public String BirthCity;
	public String BirthCountryCode;
	public String Gender;
	public String CivilStatus;
	public String Citizenship;
	public String CommonRefNo;
	public Date ApplicationDate;
	public int TerminalID;
	public String TransactionRefNo;
	public String CaptureType;
	public Boolean IsMemberActive;
	public Boolean IsComplete;
	public String ApplicationRemarks;
	public String BillingCtrlNum;
	public Integer CardCount;
	public Integer CreatedBy;
	public Date CreatedDate;
	public Integer UpdatedBy;
	public Date UpdatedDate;
	public Timestamp Version;
}
