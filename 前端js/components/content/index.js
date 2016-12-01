import React, { PropTypes, Component } from 'react'
import ContentHeader from './contentHeader'
import Evaluate from '../common/evaluate'

export default class Content extends Component{
	render(){
		return(
			<div className="content">	
				<ContentHeader content={"Expense这是标题简洁好用的"}/>
				<div class="title content_title">
			        <img src="./img/Avatar.gif" alt="Avatar">
			        <div class="subtitle">
			            <h2>这是用户名</h2>
			            <ul class="label">
			                <li> <p> # </p> 这是标签 </li>
			                <li> <p> • </p>这是用户名</li>
			                <li> <p> • </p>25分钟以前</li>
			                <div class="clr"></div>
			            </ul>
			        </div>
			        <div class="clr"></div>
			    </div>
			    <div class="content_main">
			        <div class="read">
			            <h2>Expense这是标题简洁好用的标题完整版要多完整就多完整！</h2>
			            <p>很多人都不知道是因为它木有安卓客户端，每天分享的图片和文字及音乐让人心旷神怡。还可以在特别的日子分享给对你重要的人~
			                但是你以为它仅仅是一款小清新应用吗？我随机挑两天的图片举个例子。
			                <img src="./img/showimg.jpg" alt="">
			                可追踪你的跑步并帮助你达成跑步目标 -- 不管你是想赢得第一次比赛，还是创下个人新纪录。 不管你是新手跑者或马拉松老将，都将取得数据追踪和跑步动力，激励自己跑得比以往更远更快。 欢迎来到全球最棒的跑步社区。
			            </p>
			        </div>
			        <Evaluate />
			    </div>
			</div>	
		)
	}
}