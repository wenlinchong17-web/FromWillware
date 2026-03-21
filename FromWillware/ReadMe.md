# 项目整体架构

## 📁 目录结构

```
Assets/
├── Scripts/
│   ├── Core/              # 核心系统
│   ├── Character/
│   │   ├── Base/          # Player 与 Enemy 的共同机制
│   │   ├── Player/
│   │   ├── Enemy/
│   ├── Camera/            # 与镜头有关
│   ├── UI/
│   ├── Data/
│
├── Resources/             # 项目的各种资源
├── Animations/            # 存放项目动画
│   ├── Player/
│   ├── Enemy/
├── Materials/
├── Scenes/                # 存放场景
```

---

## 📌 说明

- `Scripts/Core/`：存放核心系统代码（如状态机、输入等）
- `Scripts/Character/`：角色相关代码  
  - `Base/`：基础角色逻辑  
  - `Player/`：玩家逻辑  
  - `Enemy/`：敌人逻辑  
- `Camera/`：摄像机控制相关代码  
- `UI/`：界面相关代码  
- `Data/`：数据相关（如 ScriptableObject）  

- `Resources/`：项目资源文件  
- `Animations/`：动画文件（按 Player / Enemy 分类）  
- `Materials/`：材质文件  
- `Scenes/`：Unity 场景文件  

---

## ⚠️ 注意

在 `Character/Base` 中存在：

```
Character.cs
```

👉 **要求：**

- `Player` 和 `Enemy` 必须继承 `Character` 类  
- 所有角色的通用逻辑应写在该基类中  