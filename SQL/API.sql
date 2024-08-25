CREATE VIEW SponsorsWithPaymentDetails AS SELECT 
    s.SponsorID,
    s.SponsorName,
    s.IndustryType,
    s.ContactEmail,
    s.Phone,
    SUM(p.AmountPaid) AS TotalPayments,
    CAST(COUNT(p.PaymentID) AS INT) AS NumberOfPayments,
    MAX(p.PaymentDate) AS LatestPaymentDate
FROM sponsor.Sponsors s
JOIN sponsor.Contracts c ON s.SponsorID = c.SponsorID
JOIN sponsor.Payments p ON c.ContractID = p.ContractID
GROUP BY s.SponsorID, s.SponsorName, s.IndustryType, s.ContactEmail, s.Phone
ORDER BY sponsorid;

DROP VIEW SponsorsByMatchCountAsPerYear;

SELECT * FROM sponsor.Payments;

CREATE VIEW MatchesWithTotalPayments AS SELECT m.matchid, 
	m.matchname, 
	m.matchdate, 
	m.location ,
	SUM(p.amountpaid) AS TotalPayments
	FROM sponsor.Matches m
	JOIN sponsor.Contracts c ON c.matchid = m.matchid
	JOIN sponsor.Payments p ON c.contractid = p.contractid
	GROUP BY m.matchid, m.matchname, m.matchdate, m.location
	ORDER BY m.matchid;


CREATE VIEW SponsorsByMatchCountAsPerYear AS
SELECT S.SponsorID,S.SponsorName, CAST(COUNT(M.MatchID) AS INT) AS MatchCount,
	m.MatchDate
FROM sponsor.Sponsors S
JOIN sponsor.Contracts C ON S.SponsorID = C.SponsorID
JOIN sponsor.Matches M ON C.MatchID = M.MatchID
WHERE date_part('year',m.MatchDate) = 2024
GROUP BY S.SponsorID,m.MatchDate;

SELECT * FROM SponsorsByMatchCountAsPerYear 
	WHERE date_part('year',matchdate) = 2024;

SELECT * FROM sponsor.Payments;