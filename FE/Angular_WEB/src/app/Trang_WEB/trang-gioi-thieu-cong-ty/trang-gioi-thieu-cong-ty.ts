import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-gioi-thieu-cong-ty',
  imports: [DatePipe, CommonModule, HeaderWEB],
  templateUrl: './trang-gioi-thieu-cong-ty.html',
  styleUrl: './trang-gioi-thieu-cong-ty.css'
})
export class TrangGioiThieuCongTy implements OnInit{
  loading = false;
  following = false;
  activeTab: string = 'overview';
  company: any;

  ngOnInit(): void {
    // Giả lập tải dữ liệu
    this.loading = true;
    setTimeout(() => {
      this.company = {
        id: 1,
        name: 'Công ty TNHH Giải pháp Công nghệ ABC',
        logoUrl: '/assets/logos/abc-tech.png',
        website: 'https://www.abc-tech.vn',
        industry: 'Công nghệ thông tin',
        location: 'Hà Nội, Việt Nam',
        description: `
          <p><strong>ABC Tech</strong> là công ty hàng đầu trong lĩnh vực phát triển phần mềm, 
          giải pháp chuyển đổi số và tư vấn công nghệ tại Việt Nam.</p>
          <p>Thành lập năm 2012, hiện có hơn <b>300 nhân viên</b>, khách hàng trải dài từ ngân hàng, 
          bán lẻ, đến doanh nghiệp quốc tế.</p>
          <ul>
            <li>Văn hóa trẻ trung, năng động</li>
            <li>Cơ hội học hỏi từ các dự án lớn</li>
            <li>Đãi ngộ hấp dẫn và môi trường chuyên nghiệp</li>
          </ul>
        `,
        size: '200-500 nhân sự',
        foundedYear: 2012,
        contactEmail: 'hr@abc-tech.vn',
        totalJobs: 3,
        rating: 4.5,
        benefits: [
          'Làm việc linh hoạt (Hybrid/Remote)',
          'Chế độ bảo hiểm sức khỏe toàn diện',
          'Du lịch hàng năm',
          'Thưởng cuối năm hấp dẫn'
        ],
        featuredJobs: [
          {
            id: 101,
            title: 'Lập trình viên Angular/TypeScript',
            location: 'Hà Nội',
            salary: '20-30 triệu/tháng',
            postedAt: '2025-10-20'
          },
          {
            id: 102,
            title: 'Kỹ sư DevOps',
            location: 'Hà Nội',
            salary: '30-40 triệu/tháng',
            postedAt: '2025-10-18'
          }
        ],
        jobs: [
          {
            id: 101,
            title: 'Lập trình viên Angular/TypeScript',
            location: 'Hà Nội',
            salary: '20-30 triệu/tháng',
            postedAt: '2025-10-20'
          },
          {
            id: 102,
            title: 'Kỹ sư DevOps',
            location: 'Hà Nội',
            salary: '30-40 triệu/tháng',
            postedAt: '2025-10-18'
          },
          {
            id: 103,
            title: 'Chuyên viên QA Automation',
            location: 'Hà Nội',
            salary: '15-25 triệu/tháng',
            postedAt: '2025-10-15'
          }
        ],
        reviews: [
          {
            author: 'Nguyễn Văn A',
            date: '2025-09-05',
            content: 'Môi trường làm việc thân thiện, nhiều cơ hội học hỏi.'
          },
          {
            author: 'Trần Thị B',
            date: '2025-09-10',
            content: 'Quy trình rõ ràng, tuy hơi áp lực vào giai đoạn cuối dự án.'
          }
        ]
      };
      this.loading = false;
    }, 800);
  }

  toggleFollow() {
    this.following = !this.following;
  }

  openJob(job: any) {
    alert(`Xem chi tiết việc làm: ${job.title}`);
  }
}
