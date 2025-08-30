import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
declare var bootstrap: any;

@Component({
  selector: 'app-tin-tuyen-dung',
  imports: [CommonModule, FormsModule],
  templateUrl: './tin-tuyen-dung.html',
  styleUrl: './tin-tuyen-dung.css'
})
export class TinTuyenDung {
  posts = [
    { id:1, userName: 'Ngọc Công', avatar: 'avatar1.png', timeAgo: '1h ago', content: 'Nội dung bài 1', likes: 0, comments: 0 },
    { id:2, userName: 'Nguyễn Văn A', avatar: 'avatar2.png', timeAgo: '2h ago', content: 'Nội dung bài 2', likes: 2, comments: 1 }
  ];

  selectedPost: any = null;
  newPostContent: string = '';

  // Mở popup bài hiện tại
  openPopup(post: any) {
    this.selectedPost = post;
    const modalElement = document.getElementById('postModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }

  // Mở popup đăng bài mới
  openNewPostPopup() {
    this.newPostContent = '';
    const modalElement = document.getElementById('newPostModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }

  addPost() {
    if(this.newPostContent.trim() === '') return;
    this.posts.unshift({
      id: this.posts.length + 1,
      userName: 'Bạn',
      avatar: 'avatar_default.png',
      timeAgo: 'Vừa xong',
      content: this.newPostContent,
      likes: 0,
      comments: 0
    });
    this.newPostContent = '';
    const modalElement = document.getElementById('newPostModal');
    const modal = bootstrap.Modal.getInstance(modalElement);
    modal.hide();
  }

  like(post: any) { post.likes++; }
  comment(post: any) { post.comments++; }
  share(post: any) { alert(`Shared post: ${post.id}`); }
}
