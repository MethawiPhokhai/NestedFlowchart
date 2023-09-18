# NestedFlowchart
Nested Flowchart into CPN Tools (SE Master Project)

ข้อจำกัด
1. Loop Variable (Set ใน App.config)
2. Back to connection ต้อง fix arc variable
3. กรณีเป็น Array ต้องใช้ List แทน เพราะข้อจำกัดของ CPN Tools
4. ใน Code Segment Inscription output ต้องเป็นตัวแปรใหม่
5. ใช้ <br> ในการแบ่ง การประกาศตัวแปรใน Flowchart
6. ใช้ ++ เป็นตัวบ่งบอกว่าออกจาก Itteration (เพื่อลาก Arc ไปหา Output port)
7. Arrow ของ Condition (True/False) ตอนสร้างใน Draw.io ต้องใช้ Connect with Label และใส่ True False บนเส้น
   
การใช้งาน Software
1. สร้าง Flowchart จาก Draw.io
2. Export เป็น XML
3. นำมา Import
4. กด Transform เพื่อดู Element ทั้งหมด
5. เลือก Path ที่จะ Export เป็น CPN Tools
6. กด Export
7. เปิดไฟล์ CPN Tools ที่ Export และทดลอง Simulate เพื่อดูผลลัพธ์

Run program

แก้ไข TemplatePath ที่ App.config ก่อน ให้ชี้ไปหา Template Path ใน Project Folder