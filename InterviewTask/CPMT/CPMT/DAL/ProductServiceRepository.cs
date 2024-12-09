using CPMT.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CPMT.DAL
{
    public class ProductServiceRepository
    {
        public class MeetingMinutesRepository
        {
            private readonly GTwoTaskEntities _context;

            public MeetingMinutesRepository(GTwoTaskEntities context)
            {
                _context = context;
            }

            public int SaveMeetingMinutesMaster(Meeting_Minutes_Master_Tbl meetingMinutesMaster)
            {
                var meetingIDParam = new SqlParameter("@MeetingID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                _context.Database.ExecuteSqlCommand(
                    "EXEC Meeting_Minutes_Master_Save_SP @CustomerID, @CustomerType, @MeetingDate, @MeetingTime, @MeetingPlace, @AttendsFromClientSide, @AttendsFromHostSide, @MeetingAgenda, @MeetingDiscussion, @MeetingDecision, @MeetingID OUT",
                    new SqlParameter("@CustomerID", meetingMinutesMaster.CustomerID),
                    new SqlParameter("@CustomerType", meetingMinutesMaster.CustomerType),
                    new SqlParameter("@MeetingDate", meetingMinutesMaster.MeetingDate),
                    new SqlParameter("@MeetingTime", meetingMinutesMaster.MeetingTime),
                    new SqlParameter("@MeetingPlace", meetingMinutesMaster.MeetingPlace),
                    new SqlParameter("@AttendsFromClientSide", meetingMinutesMaster.AttendsFromClientSide),
                    new SqlParameter("@AttendsFromHostSide", meetingMinutesMaster.AttendsFromHostSide),
                    new SqlParameter("@MeetingAgenda", meetingMinutesMaster.MeetingAgenda),
                    new SqlParameter("@MeetingDiscussion", meetingMinutesMaster.MeetingDiscussion),
                    new SqlParameter("@MeetingDecision", meetingMinutesMaster.MeetingDecision),
                    meetingIDParam
                );

                return (int)meetingIDParam.Value;
            }

            public void SaveMeetingMinutesDetails(IEnumerable<Meeting_Minutes_Details_Tbl> meetingMinutesDetails)
            {
                foreach (var detail in meetingMinutesDetails)
                {
                    _context.Database.ExecuteSqlCommand(
                        "EXEC Meeting_Minutes_Details_Save_SP @MeetingID, @ProductServiceID, @Quantity, @Unit",
                        new SqlParameter("@MeetingID", detail.MeetingID),
                        new SqlParameter("@ProductServiceID", detail.ProductServiceID),
                        new SqlParameter("@Quantity", detail.Quantity),
                        new SqlParameter("@Unit", detail.Unit)
                    );
                }
            }
        }
    }
}