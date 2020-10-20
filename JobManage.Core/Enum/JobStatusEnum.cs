using System.ComponentModel;

namespace JobManage.Core
{
    public enum JobStatusEnum
    {
        [Description("待执行")]
        ReadyRun = 1,
        [Description("执行中")]
        Running = 2,
        [Description("待暂停")]
        ReadyPause = 3,
        [Description("已暂停")]
        Paused = 4,
        [Description("待恢复")]
        ReadyRecover = 5,
        [Description("待删除")]
        ReadyDelete = 6,
        [Description("已删除")]
        Deleted = 7
    }
}
