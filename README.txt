作成者：新沼天河


目次
[1.概要]
[2.環境]
[3.ディレクトリ構成]


-----1.概要-----

このプロジェクトについて
 -> 自作ゲーム「CycleSkillFight」のスクリプト集です。
 -> Unityを使用しています。

ゲームの内容
 -> ジャンル　 - コマンド選択式PvPゲーム
 -> プレイ人数 - 1～2人

紹介動画
https://drive.google.com/file/d/1LdOTVqty2Syl7iw7Hebvgp30HYq1HoaY/view?usp=sharing


-----2.環境-----

OS  -> Windows 11 Home
CPU -> 13th Gen Intel(R) Core(TM) i5-13400F 2.50 GHz
RAM -> 32.0 GB

エンジン -> Unity (2022.3.3f1)
言語　　 -> C#

ライブラリ -> Photon Unity Networking 2 (2.39)
(PUN2を使用している部分がありますが、PUN2のデータは含まれていません。)


-----3.ディレクトリ構成-----

├─GameSystem
│  │  InputManager.cs
│  │
│  └─Data
│          EnumList.cs
│          FighterDatas.cs
│          FighterSkillData.cs
│          MyConst.cs
│          SkillData.cs
│          SkillDictionary.cs
│
├─InGame
│  ├─Battle
│  │      BattleManager.cs
│  │      FighterController.cs
│  │      FighterSkillSetting.cs
│  │      FighterStatus.cs
│  │      NW_DataMessenger.cs
│  │      NW_FighterController.cs
│  │
│  └─Skills
│          AttackParents.cs
│          SkillParent.cs
│          Skill_AtkUpAttack.cs
│          Skill_BackSlash.cs
│          Skill_BackStep.cs
│          Skill_Charge.cs
│          Skill_Gurd.cs
│          Skill_heavyAttack.cs
│          Skill_NormalSlashing.cs
│          Skill_NormalStab.cs
│
├─OutGame
│   └─UI
│           InputObserver.cs
│           NW_LogInManager.cs
│           NW_SceneChangeManager.cs
│           Player_SButtonData.cs
│           SceneChangeManager.cs
│           Select_SButtonData.cs
│           SkillSelectController.cs
│           TitleInputController.cs
│           TitlleManager.cs
└─ SimpleSingleton.cs
