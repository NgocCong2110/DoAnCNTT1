import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../services/auth';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-the-banner',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './the-banner.html',
  styleUrls: ['./the-banner.css']
})
export class TheBanner implements OnInit {
  banners: any[] = [];

  constructor(public auth: Auth) { }

  ngOnInit(): void {
    this.banners = [
      {
        tieu_de: "Khám Phá Cơ Hội Nghề Nghiệp",
        mo_ta: "Chúng tôi giúp bạn tìm kiếm hàng ngàn cơ hội việc làm phù hợp với kỹ năng, kinh nghiệm và sở thích của bạn. Ứng tuyển dễ dàng, nhận việc nhanh chóng và phát triển sự nghiệp bền vững.",
        img: "anh_WEB/anh_Banner.jpg",
        show: true,
        button: null
      },
      {
        tieu_de: "Tạo CV ngay bây giờ",
        mo_ta: "Tạo CV chuyên nghiệp để nổi bật trước nhà tuyển dụng và tăng cơ hội được mời phỏng vấn.",
        img: "anh_WEB/anh_Banner4.png",
        show: true,
        button: { text: "Tạo CV", link: "/trang-tao-cv" }
      },
      {
        tieu_de: "Đăng Ký Hôm Nay",
        mo_ta: "Tham gia ngay để khám phá cơ hội việc làm phù hợp đam mê và phát triển sự nghiệp.",
        img: "anh_WEB/anh_Banner2.jpg",
        show: !this.auth.layThongTinNguoiDung(),
        button: { text: "Đăng Ký Ngay", link: "/dang-ky" }
      },
      {
        tieu_de: "Tuyển Dụng Nhanh Chóng",
        mo_ta: "Đăng ký để đăng tin tuyển dụng và tìm ứng viên phù hợp dễ dàng.",
        img: "anh_WEB/anh_Banner3.jpg",
        show: !this.auth.layThongTinNguoiDung(),
        button: { text: "Đăng Ký Ngay", link: "/dang-ky-cong-ty" }
      }
    ];
  }

}
