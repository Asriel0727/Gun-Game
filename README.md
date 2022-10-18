# 第一人稱射擊遊戲
配合Unity 3D遊戲設計實戰書籍製作
## 遊戲基本介紹
遊戲內有兩種敵人：哨兵槍及機器人，當機器人被攻擊時會顯示血條；被打倒時會有倒下動作，遊戲通關方式為拿到所有寶物並開啟密室後取得軍旗。  
內部設計有：開關門碰撞設定、奪取寶物、機器人AI控制、途徑點及場景設置、哨兵槍的偵測及攻擊、製作雷達、遊戲暫停及通關等等
 

## 架構圖
![架構圖](https://user-images.githubusercontent.com/96182969/196508039-d1b3ac92-f6d5-4f7f-91d2-15fe6f4d2ed1.png)
## 武器架構圖  
![武器架構 drawio](https://user-images.githubusercontent.com/96182969/196510456-b16481db-45a1-43d7-b699-b511dda14579.png)
## 相關程式碼

### OpenDoor
設定是否開門、開門時間、音效、動畫

### PlayerWeapons
初始內定武器，武器選擇以及發射設置

### Rocket
製作火箭彈並給予碰撞後的效果及物件刪除
主要利用OnCollisionEnter偵測碰撞

### RocketLauncher
腳本參數

運用動態生成產生要發射的火箭彈，並給予飛行方向及速度

### MachineGun
腳本參數

主要依靠Physics.Raycast偵測是否打到物品  
定義彈藥數量、射擊特效及音效，如果子彈=0則會裝填子彈，以及射出後碰撞到物品的火花特效

### DamageReceiver
腳本參數

製作爆炸桶，內有接受傷害訊息接收處，接受訊息後減少生命值，當生命值歸零後執行死亡事件，並產生爆炸特效且套入材質球，使其有被炸毀的感覺，後產生被炸毀的汽油桶

### SentryGun
腳本參數

製作哨兵槍(定點敵人，對玩家攻擊)簡單AI  
(1)判斷玩家是否在攻擊範圍內，且未被障礙物阻擋  
(2)Quaternion.LookRotation & Quaternion.Slerp 使機槍能夠轉向玩家，追蹤玩家方向  
(3)當玩家在攻擊範圍內，且符合射擊角度，就傳送開火訊息給MachineGun，對玩家攻擊  

### AutoWayPoint
腳本參數

OnDrawGizmos可以在Scene視窗中顯示圖案，但不會顯示在遊戲中  
Gizmos.DrawIcon第一個參數表顯示的位置，第二個參數為顯示圖片的檔名  

### AI
腳本參數

內部設置行為模式：巡邏、發現目標、對目標攻擊  

### EnemyDamage
腳本參數

建立機器人生命值  

### FPSPlayer
腳本參數

建立主角的UI及受傷死亡音效  
(與EnemyDamage相同，有接受傷害訊息的接收處，並且把訊息傳送到UI)  

### RotateObject
腳本參數

使物體旋轉，利用transform.Rotate語法

### BoxCollect
建立玩家撿取寶物
(和開關門作法相似，當玩家碰到寶物後，將寶物刪除，並計數在一個全域變數中)  
將撿取寶物的數量紀錄在GetCount中，利用CountDisplay讀取，並顯示在GUI Text中  

### Radar
腳本參數

雷達功能:雷達底圖，敵人底圖，雷達中心點物件

### GamePause
建立遊戲暫停  
遊戲暫停是由Time.timeScale控制(時間速度控制)  
1為正常速度  
0為停止
(But 避免出BUG將0參數設為0.0001)

### BlurEffect
使鏡頭產生物化的特效

### TriggerLoadScene
讀取連結的關卡

### GamePass
設定過關條件

### LookCamera
設定Camera開啟以及視角位置

### LevelLoadFade
建立紋理淡入/出

### Explosion
宣告:爆炸範圍、力量、傷害值、結束時間
對爆炸範圍內目標(ApplyDamage)發送傷害值，並對Rigidbody發送爆炸力量
