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
public class MemberEmployementHistory {
	@Id
	public Integer ID;
	public UUID GUID;
	public String HistoryEmployerName;
	public String HistoryEmployerAddress;
	public Date HistoryEmployerDateEmployed;
	public Date HistoryEmployerDateSeperated;
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
