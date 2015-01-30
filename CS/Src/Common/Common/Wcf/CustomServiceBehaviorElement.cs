using System;
using System.ServiceModel.Configuration;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// app.configに登録できるようにするためのエレメント
    /// </summary>
    //************************************************************************
    public class CustomServiceBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CustomServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new CustomServiceBehavior();
        }
    }
}
