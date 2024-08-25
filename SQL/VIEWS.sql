SHOW search_path;
SET search_path = sponsor;

-- 1 Create a view that lists all matches along with their sponsors and the total payments received. 
-- Then, retrieve all matches where the total payments exceed $200,000.

CREATE VIEW MatchSponsorPayments AS
SELECT * FROM 
	(SELECT m.matchname,s.sponsorname, 
		COALESCE(SUM(P.AmountPaid), 0) AS TotalPayments
		FROM Matches m 
		JOIN Contracts c ON m.matchid = c.matchid
		JOIN Sponsors s ON c.sponsorid = c.sponsorid
		JOIN Payments p ON p.contractid = c.contractid
		GROUP BY m.matchid,s.sponsorid)
;

SELECT * FROM MatchSponsorPayments 	
	WHERE TotalPayments > 200000
;

-- 2 Create a view that displays the contract details (ContractID, SponsorName, MatchName, ContractValue) 
-- and write a query to retrieve contracts with a value greater than the average contract value.

CREATE VIEW ContractDetails AS
SELECT 
    C.ContractID,
    S.SponsorName,
    M.MatchName,
    C.ContractValue
FROM Contracts C
JOIN Sponsors S ON C.SponsorID = S.SponsorID
JOIN Matches M ON C.MatchID = M.MatchID;

SELECT *
FROM ContractDetails
WHERE ContractValue > (SELECT AVG(ContractValue) FROM ContractDetails);

-- 3 Create a view to display all pending payments for each sponsor and write a query to list 
-- the sponsors who have pending payments totaling more than $50,000.

CREATE VIEW PendingPaymentsView AS
SELECT 
    S.SponsorName,
    P.ContractID,
    SUM(P.AmountPaid) AS PendingPayments
FROM Sponsors S
JOIN Contracts C ON S.SponsorID = C.SponsorID
JOIN Payments P ON C.ContractID = P.ContractID
WHERE P.PaymentStatus = 'Pending'
GROUP BY S.SponsorName, P.ContractID;

SELECT * FROM PendingPaymentsView 
	WHERE PendingPayments>50000;

-- 4 Create a view that shows the number of matches each sponsor has sponsored. 
-- Then, write a query to find sponsors who have sponsored more than 2 matches.
CREATE VIEW SponsorMatchCount AS
SELECT 
    S.SponsorName,
    COUNT(DISTINCT C.MatchID) AS MatchesSponsored
FROM Sponsors S
JOIN Contracts C ON S.SponsorID = C.SponsorID
GROUP BY S.SponsorName;

SELECT * FROM SponsorMatchCount WHERE MatchesSponsored>2;

-- 5 Create a SQL view that provides a comprehensive overview of all sponsorship activities. 
-- The view should include the following details: Sponsor Details: Sponsor Name, Industry Type,
-- Contact Email, Match Details, Match Name, Match Date, Location, Contract Details,
-- Contract Value, Contract Date, Payment Summary, Total Amount Paid by the Sponsor, Number of Payments Made,
-- Latest Payment Date Additional Requirements: Only include sponsors who have made at least one payment. 
-- Calculate the total amount paid by each sponsor for each match. 
-- Include sponsors even if they have contracts but no completed payments, indicating their status with a Pending label 
-- in the PaymentStatus column. Sort the results first by Sponsor Name and then by Match Date.

CREATE VIEW SponsorshipOverview AS;
SELECT 
    S.SponsorName,S.IndustryType,S.ContactEmail,M.MatchName,M.MatchDate,M.Location,C.ContractValue,C.ContractDate,
    SUM(P.AmountPaid) AS TotalAmountPaid,
    COUNT(P.PaymentID) AS NumberOfPaymentsMade,
    CASE
        WHEN MAX(P.PaymentDate) IS NOT NULL THEN TO_CHAR(MAX(P.PaymentDate), 'YYYY-MM-DD')
        ELSE 'No Payments'
    END AS LatestPaymentDate,
    CASE
        WHEN COUNT(P.PaymentID) > 0 THEN 'Completed'
        ELSE 'Pending'
    END AS PaymentStatus
FROM Sponsors S
JOIN Contracts C ON S.SponsorID = C.SponsorID
JOIN Matches M ON C.MatchID = M.MatchID
LEFT JOIN Payments P ON C.ContractID = P.ContractID
GROUP BY S.SponsorName,S.IndustryType,S.ContactEmail,M.MatchName,M.MatchDate,M.Location,C.ContractValue,C.ContractDate
HAVING 
    SUM(P.AmountPaid) > 0 OR COUNT(P.PaymentID) = 0
ORDER BY 
    S.SponsorName ASC, 
    M.MatchDate ASC;