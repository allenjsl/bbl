<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookDetail.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.LookDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>行政中心-员工本月考勤明细</title>
    <style type="text/css">
        body {
	        font: 100% Verdana, Arial, Helvetica, sans-serif;
	        background: #666666;
	        margin: 0; 
	        padding: 0;
	        text-align: center; 
	        color: #000000;
        }
        #calendarContainer {
	        width: 700px; 
	        background: #FFFFFF;
	        margin: 0 auto; 
	        text-align: left;
        }
        #calendarContainer a{
        	font-size:15px;
        	font-weight:bold;
        }
    </style>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/AttCalendar.js" type="text/javascript"></script>
    <script type="text/javascript">

        var LookDetail = {
            QueryString: function(val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            },
            LookWorker: function(day, id) {
                var iframeId = LookDetail.QueryString("iframeid");
                parent.Boxy.iframeDialog({ title: day + "--员工考勤明细",
                    iframeUrl: "/administrativeCenter/attendanceManage/UpdateRecord.aspx",
                    width: "640px", height: "520px",
                    model: true, data: {
                        desid: iframeId,
                        WorkerID: id,
                        DateTime: day
                    }
                });
            },
            SetMonthCalendar: function(year, month) {           //获取下一页的数据
                var reResult;
                $.newAjax({
                    type: "GET",
                    dateType: "html",
                    url: "/administrativeCenter/attendanceManage/LookDetail.aspx",
                    data: {
                        dataYear: year,
                        dataMonth: month,
                        method: 'SetMonthCalendar',
                        WorkerID: $("#<%=hiddenWorkerID.ClientID %>").val()
                    },
                    cache: false,
                    async: false,
                    success: function(result) {
                        reResult = result;
                    }
                });

                return reResult;
            }
        };
        $(function() {
            ChildTours.initCalendar({
                configObj: { beginDate: new Date(), json: $("#<%=hiddenJson.ClientID %>").val() }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" >
    <div id="calendarContainer">
    </div>
    <%--<table width="700" border="0" align="center" cellpadding="0" cellspacing="0" class="">
      <tr>
        <td> <strong>本年：</strong>准点<font color="#006633"><strong>5</strong></font>天，迟到<font color="#006633"><strong>2</strong></font>天，早退<font color="#006633"><strong>0</strong></font>天，旷工<font color="#006633"><strong>0</strong></font>天，休假<font color="#006633"><strong>0</strong></font>天，请假<font color="#006633"><strong>2</strong></font>天，加班<font color="#006633"><strong>0.0</strong></font>小时，外出<strong><font color="#FF0000">0.0</font></strong>天，出团<font color="#FF0000"><strong>0.0</strong></font></td>
      </tr>
      <tr>
        <td><strong>本月：</strong>准点<font color="#FF0000"><strong>1</strong></font>天，迟到<font color="#FF0000"><strong>0</strong></font>天，早退<font color="#FF0000"><strong>0</strong></font>天，旷工<font color="#FF0000"><strong>0</strong></font>天，休假<font color="#FF0000"><strong>0</strong></font>天，请假<font color="#FF0000"><strong>0</strong></font>天，加班<font color="#FF0000"><strong>0.0</strong></font>小时，外出<font color="#FF0000"><strong>0.0</strong></font>天，出团<font color="#FF0000"><strong>0.0</strong></font></td>
      </tr>
    </table>--%>
    <input id="hiddenJson" type="hidden" runat="server" />
    <input id="hiddenWorkerID" type="hidden" runat="server" />
    </form>
</body>
</html>
