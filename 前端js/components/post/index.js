import React, { PropTypes, Component } from 'react'

export default class Post extends Component{
	render(){
		return(
			<div className="post_all">
			    <div className="notice_head">
			        <a href="index.html">取消</a>
			        <div className="clr"></div>
			    </div>
			    <div className="input_main">
			    	//此处应加一个富文本编辑器
			        <textarea maxlength="300" placeholder="想说点什么......"></textarea>
			    </div>

			    <div className="post_footer">

			        <ul className="icon">
			            <li><img src="./img/envelope.png" alt=""></li>
			            <li><img src="./img/smiley.png" alt=""></li>
			            <li><img src="./img/photography.png" alt=""></li>
			            <div className="clr"></div>

			        </ul>
			        <ul className="icon_input">
			            <li><input type="file" accept="image/*;capture=camera"></li>
			            <li><input type="file" accept="image/*;capture=camera"></li>
			            <li><input type="file" accept="image/*;capture=camera"></li>
			            <div className="clr"></div>
			        </ul>
			        <button className="post">发表</button>
			    </div>
			</div>
		)
	}
}