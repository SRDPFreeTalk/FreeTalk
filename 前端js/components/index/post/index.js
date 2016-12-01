import React, { PropTypes, Component } from 'react'

export default class Post extends Component{
	render(){
		const content = this.props;
		return (
			<div className="article">
	            <div className="title">
	                <img src="./img/Avatar.gif" alt="Avatar">
	                <div className="subtitle">
	                    <h2><a href="content.html"> Expense这是标题简洁好用的.Expense这是标题简洁好用的Expense这是标题简洁好用的Expense这是标题简洁好用的..</a></h2>
	                    <ul className="label">
	                        <li>
	                            <p> # </p> 这是标签 </li>
	                        <li>
	                            <p> • </p>这是用户名</li>
	                        <li>
	                            <p> • </p>25分钟以前</li>
	                        <div className="clr"></div>
	                    </ul>
	                </div>
	                <div className="clr"></div>
	            </div>
	            <div className="img">
	                <img src="./img/showimg.jpg" alt="">
	            </div>
	            <div className="article_content">
	                <p><a href="content.html">“钢铁骑士”曾是美泰一款风行全球的男孩人偶玩具，也曾被拍成电视系列动画片，此次真人电影版的故事是《雷神2：黑暗世界》</a></p>
	            </div>
	            <div className="evaluate">
	                <button className="thumbs_up"></button>
	                <p>800</p>
	                <button className="chat"></button>
	                <p>600</p>
	                <div className="clr"></div>
	            </div>
	            <div className="clr"></div>
	        </div>
		)
	}
}