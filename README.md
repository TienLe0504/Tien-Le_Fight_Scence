# Main scene: Assets\Resources\EZ Assets\Scenes\Tien-Le_IB-Fight-Scene.unity
# File Name: “EZ-Test_Tien-Le_IB-Fight-Scene”.
# Designed By: Lê Văn Tiền
- Link to file apk: https://drive.google.com/file/d/1fg5L8pZEHvImhB7PFi9Pa165S7igzMxi/view?usp=sharing
- Link to file mp4: https://drive.google.com/file/d/1Ipj2cWGAV5EC08ty7K606OE9XEhmsNCA/view?usp=sharing
- Link to Git: https://github.com/TienLe0504/Tien-Le_Fight_Scence
  
# GIẢI THÍCH THUẬT TOÁN CÂU 2
# YÊU CẦU 1
## 1.1 Mục tiêu
- Phân loại: Chia 10 level thành 3 game mode riêng biệt để tạo sự đa dạng.
- Độ khó tăng dần: Mỗi level sau sẽ khó hơn level trước.
 
## 1.2 Cách phân chia level:
- Lấy tổng số level(10) chia cho số game mode (3) = 10 /3 = 3 (dư 1).
- Kết quả: mỗi game mode sẽ có 3 level.
- Level dư (1) được cộng vào game mode cuối cùng.
 
## 1.3  Kết quả sau phân chia:
- Game mode (1 vs 1): 3 level (1, 2, 3).
- Game mode (1 vs Many): 3 level (4, 5, 6).
- Game mode (Many vs Many): 4 level (7, 8, 9, 10)
 
## 1.4 Giới thiệu các thuộc tính của kẻ địch (và đồng đội):
- Health: máu
- damage: sát thương
- attackCoolDown: Thời gian tung ra đòn đánh kế tiếp
- drag: Lực cản di chuyển
- timeToMove: thời gian kẻ địch di chuyển liên tục trước khi dừng lại nghỉ
- timeToIdle: thời gian đứng nghỉ giữa các hành động
- canAttackContinuously: Tấn công liên tục, không có trạng thái nghỉ.
- timeAttackContinue: thời gian kẻ địch tiếp tục giữ đòn tấn công nếu người chơi rời khỏi tầm đánh.
 
## 1.5 Phân tích cơ chế tăng độ khó của game
Độ khó của game được tăng lên qua từng level, không ngẫu nhiên. Gồm 3 yếu tố chính:
- Sức mạnh: Chỉ số máu, sát thương đều tăng.
- Tốc độ: Nhanh hơn, ít thời gian nghỉ hơn.
- Độ phức tạp của màn chơi: Tăng số lượng kẻ địch, thay đổi game mode.
### 1.5.1 Sức Mạnh
Đây là phương pháp trực tiếp nhất để tăng độ khó. Các chỉ số cốt lõi của kẻ địch (và đồng đội) đều được tăng tuyến tính theo currentLevel.
- Health:
Công thức: health = baseHealth + (currentLevel * factor) (Ví dụ game mode 1vs1: 80f + (currentLevel * 10f)).
Tác động: kẻ địch ngày càng trâu bò hơn, do đó cần phải tấn công nhiều hơn để hạ gục. Bên cạnh đó trận đấu sẽ dài hơn, người chơi có thể mắc sai lằm nhiều hơn
- damage:
Công thức: damage = baseDamage + (currentLevel * factor) (Ví dụ game mode 1vs1: 8f + (currentLevel * 1.5f)).
Tác động:  Ở level cao, mỗi đòn đánh gây sát thương lớn hơn, buộc người chơi phải né đòn tốt và ra quyết định nhanh hơn.
- drag:
Công thức: drag = Mathf.Max(minDrag, baseDrag - (currentLevel * factor))
Tác động: Chỉ số drag càng thấp, đối tượng càng di chuyển nhanh hơn, theo đuổi người chơi liên tục →  gây áp lực lớn cho người chơi.
### 1.5.2. Tốc độ
- attackCoolDown:
Công thức: attackCooldown = Mathf.Max(minCooldown, baseCooldown - currentLevel * factor)
Tác động: Khi level tăng, thời gian chờ giữa các đòn tấn công của kẻ địch giảm xuống. Chúng tấn công nhanh hơn, dày đặc hơn, khiến người chơi có ít cơ hội  để tấn công và nghỉ hơn.
- timeToIdle:
Công thức: timeToIdle = Mathf.Max(minIdle, baseIdle - i * factor)
Tác động: Thời gian kẻ địch đứng nghỉ bị rút ngắn. Chúng gần như luôn trong trạng thái di chuyển để bám theo người chơi. Điều này gây áp lực cho người chơi.
- canAttackContinuously:
Tác động: Cơ chế kích hoạt cho kẻ địch bám sát tấn công liên tục, không có thời gian đứng nghỉ. Gây áp lực lớn cho người chơi.
- timeToMove:
Tác động: Thời gian càng nhiều  → thì kẻ địch càng lâu để chuyển qua trạng thái đứng nghỉ,
- timeAttackContinue:
Tác động: Thời gian càng ít,  kẻ địch hủy đòn đánh nhanh hơn → Tấn công người chơi liên tục, người chơi ít có cơ hội tấn công. Gây khó khăn cho người chơi.
### 1.5.3 Độ phức tạp của màn chơi.
Số lượng kẻ địch: Chuyển từ game mode 1 vs 1 lên nhiều kẻ địch ( 1 vs Many, Many vs Many).
Tác động: Kẻ địch đông hơn, người chơi phải xử lý nhiều mối đe dọa cùng lúc.
Xuất hiện của đồng đội:
Tác động: Người chơi không chỉ chiến đấu một mình mà còn phải phối hợp với đồng đội.
Bất lợi về thế trận (Game mode Many vs Many).
Tác động: Người chơi là yếu tố chính quyết định trận đấu. Ngoài đồng minh hỗ trợ, người chơi còn phải để di chuyển thông minh và tấn công mục tiêu.
 
# YÊU CẦU 2
Để đảm bảo game chạy mượt mà với gần 50 model có animation cùng lúc trên màn hình, em đã áp dụng các kỹ thuật tối ưu hóa chính sau đây.
1. Tối ưu hóa CPU và Bộ nhớ: Object Pooling
Cách thực hiện: Ngay khi bắt đầu game, em đã tạo sẵn các pool chứa đủ số lượng đối tượng cần thiết: 25 Kẻ địch, 24 Đồng minh và 49 Thanh máu (cộng thêm player nữa là đủ 50).
Cơ chế: Khi nhân vật chết, em không destroy mà chỉ ẩn đi (SetActive(false)) và đưa vào pool để tái sử dụng sau này.
Lợi ích: Tránh việc tạo và xóa đối tượng liên tục (rất tốn tài nguyên), giúp game chạy ổn định, không bị giật lag đột ngột.
2. Tối ưu hóa GPU:
Đã bật Enable GPU Instancing trên Material của các nhân vật.
3. Tối ưu hóa Vật lý (Physics)
Sử dụng Collider Đơn giản: Tất cả các nhân vật chỉ sử dụng Capsule Collider đơn giản thay vì các collider phức tạp.
Loại bỏ Va chạm không cần thiết: Đã cấu hình hệ thống vật lý để cho phép kẻ địch và đồng minh đi xuyên qua nhau.




