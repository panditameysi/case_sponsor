CREATE VIEW SponsorsWithPaymentDetails AS SELECT 
    s.SponsorID,
    s.SponsorName,
    s.IndustryType,
    s.ContactEmail,
    s.Phone,
    SUM(p.AmountPaid) AS TotalPayments,
    COUNT(p.PaymentID) AS NumberOfPayments,
    MAX(p.PaymentDate) AS LatestPaymentDate
FROM sponsor.Sponsors s
JOIN sponsor.Contracts c ON s.SponsorID = c.SponsorID
JOIN sponsor.Payments p ON c.ContractID = p.ContractID
GROUP BY s.SponsorID, s.SponsorName, s.IndustryType, s.ContactEmail, s.Phone
ORDER BY sponsorid;
