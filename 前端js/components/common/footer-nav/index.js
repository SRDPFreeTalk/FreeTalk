import React, { PropTypes, Component } from 'react'

export default class FooterNav extends Component{
	render(){
		return (
			<footer className="footer">
		        <a href="index.html">
		            <div className="setimg">
		                <img src="./img/news.png" alt="">
		                <p>帖子</p>
		            </div>
		        </a>
		        <a href="">
		            <div className="setimg">
		                <img src="./img/globe2.png" alt="">
		                <p>发现</p>
		            </div>
		        </a>
		        <a href="post.html">
		            <div className="setimg">
		                <img src="./img/pen2.png" alt="">
		                <p>发表</p>
		            </div>
		        </a>
		        <a href="notice.html">
		            <div className="setimg">
		                <img src="./img/twitter-bird2.png" alt="">
		                <p>通知</p>
		            </div>
		        </a>
		        <a href="person.html">
		            <div className="setimg">
		                <img src="./img/user2.png" alt="">
		                <p>个人</p>
		            </div>
		        </a>
		        <div className="clr"></div>
		    </footer>
		)
	}
}