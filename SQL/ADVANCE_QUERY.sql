SHOW search_path;
SET search_path = sponsor;

--1  Retrieve the total contract value for each sponsor, 
--but only for sponsors who have at least one completed payment.
--Display the Sponsorname and the TotalContractValue

select S.SponsorName, sum(C.ContractValue) as TotalContractValue
from Sponsors S
join Contracts C ON S.SponsorID = C.SponsorID
join Payments P ON C.ContractID = P.ContractID
where P.PaymentStatus = 'Completed'
group by S.SponsorName
having count(P.PaymentID) >= 1;

--2  Retrieve sponsors who have sponsored more than one match, 
--along with the total number of matches they have sponsored. 
--Display the Sponsor name and Number of Matches.

select S.SponsorName, count(distinct C.MatchID) as NumberOfMatches
from Sponsors S
join Contracts C on S.SponsorID = C.SponsorID
group by S.SponsorID
having count(distinct C.MatchID) > 1;

--3 Write an SQL query that retrieves a list of all sponsors 
--along with their total contract value.
--Additionally, categorize each sponsor based on the total value 
--of their contracts using the following criteria:
--If the total contract value is greater than $500,000, label the sponsor as 'Platinum'.
--If the total contract value is between $200,000 and $500,000, label the sponsor as 'Gold'.
--If the total contract value is between $100,000 and $200,000, label the sponsor as 'Silver'.
--If the total contract value is less than $100,000, label the sponsor as 'Bronze'.

select S.SponsorName, 
sum(C.ContractValue) as TotalContractValue,
case
	when sum(C.ContractValue) > 500000 then 'Platinum'
	when sum(C.ContractValue) between 200000 and 500000 then 'Gold'
	when sum(C.ContractValue) between 100000 and 200000 then 'Silver'
else 'Bronze'
end 
	as Category
from Sponsors S
join Contracts C on S.SponsorID = C.SponsorID
group by S.SponsorName
order by TotalContractValue DESC;

--4 Retrieve Matches Where the Average Contract Value is Greater Than the Average 
--Contract Value of All Matches. 
--Display the match name and average contract value.

select M.MatchName, avg(C.ContractValue) as AverageContractValue
from Matches M
join Contracts C on M.MatchID = C.MatchID
group by M.MatchID, M.MatchName
having avg(C.ContractValue) > (
    select avg(C1.ContractValue)
    from Contracts C1
);

--5 Find Sponsors Who Have the Highest Total Payments for a Single Match.
--Display the sponsor name, match name and total amount paid

select S.SponsorName, M.MatchName, sum(P.AmountPaid) as TotalAmountPaid
    from Sponsors S
    join Contracts C on S.SponsorID = C.SponsorID
    join Matches M on C.MatchID = M.MatchID
    join Payments P on C.ContractID = P.ContractID
    group by S.SponsorName, M.MatchName
	order by TotalAmountPaid desc
	LIMIT 3
;

select * from sponsor.matches
