using System;

namespace FootballersCatalogue.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string User { get; set; } // ��� ������� ����������
        public string Address { get; set; } // ����� ����������
        public string ContactPhone { get; set; } // ���������� ������� ����������

        public int PhoneId { get; set; } // ������ �� ��������� ������ Phone
        public Phone Phone { get; set; }
    }
}