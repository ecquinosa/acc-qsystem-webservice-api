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
public class MemberBio {
	@Id
	public Integer ID;
	public UUID GUID;
	public String  LeftPrimaryFPTemplate;
	public Boolean LeftPrimaryFPIsOverride;
	public byte[] LeftPrimaryFPAnsi;
	public byte[] LeftPrimaryFPWsq;
	public String  LeftSecondaryFPTemplate;
	public Boolean LeftSecondaryFPIsOverride;
	public byte[] LeftSecondaryFPAnsi;
	public byte[] LeftSecondaryFPWsq;
	public String  RightPrimaryFPTemplate;
	public Boolean RightPrimaryFPIsOverride;
	public byte[] RightPrimaryFPAnsi;
	public byte[] RightPrimaryFPWsq;
	public String  RightSecondaryFPTemplate;
	public Boolean RightSecondaryFPIsOverride;
	public byte[] RightSecondaryFPAnsi;
	public byte[] RightSecondaryFPWsq;
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
