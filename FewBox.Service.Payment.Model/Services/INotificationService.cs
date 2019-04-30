using System;

namespace FewBox.Service.Payment.Model.Service
{
    public interface INotificationService
    {
        void Notify(Notification notification);
    }
}