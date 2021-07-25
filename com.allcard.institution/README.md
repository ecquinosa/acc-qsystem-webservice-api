Update
- Fix issue on generate schedule the next hour is over lapping so deduct 1 sec for the next hour.
- Issue on the last generated hour must not overlap the end time.
- Addtional cancellation of schedule by scheduleCode and date.
- Adding authorizattion feature.
- myschedule to get all the schedule generated for the branch
- Date range adjustment on get branch schedule.
- Update schedule maxperson.
- Cancel schedule also require
- Implement audit log

----------------------------
Adding new table 
RefGroupOfIsland
RefRegion
RefProvince
RefCity
