/*******************************************************************************
 * 【共通部品】
 * 
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.7.1, 新規作成
 ******************************************************************************/
using log4net.Core;
using log4net.Appender;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// VisualStudioデバッグコンソールに出力するためのAppender
    /// </summary>
    //************************************************************************
    public class DebugConsoleAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent evt)
        {
            System.Diagnostics.Debug.WriteLine(evt.RenderedMessage);
        }
    }
}
