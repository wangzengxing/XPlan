<template>
  <div id="app">
    <el-menu
      :default-active="activeIndex"
      class="el-menu-demo"
      mode="horizontal"
      router
    >
      <el-menu-item index="1">工作篮</el-menu-item>
      <el-menu-item index="2">日程管理</el-menu-item>
      <el-menu-item index="3">年度计划</el-menu-item>
    </el-menu>
    <div id="main">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {
      activeIndex: '1'
    };
  },
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  width: 60%;
  margin: 0 auto;
  margin-top: 60px;
}

#main{
  margin: 5px;
  padding: 15px;
  height: 500px;
  border: 1px solid  #888888;
  box-shadow:10px 10px 5px #888888;
}
</style>
